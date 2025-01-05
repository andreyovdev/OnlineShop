$(document).ready(function () {
    const $productCardContainer = $('.product-card-container');
    const $paginationWrapper = $('.pagination-wrapper');
    const $noProductMessage = $('.no-product-message');
    const $loadingMessage = $('.loading-message');
    const $paginationList = $paginationWrapper.find('.pagination');
    const $filterTypesContainer = $('.filter-types-container');
    const $searchInput = $('input[name="input"]');
    const $priceFilters = $('.price-filter');
    const $categoryFilters = $('.category-filter');
    const $inStockCheckbox = $('#in-stock');
    const $sortPriceFilter = $('.sort-price-filter');
    const $resetFilterBtn = $('#reset-filter-btn');
    const $searchBtn = $('.nav-input-search button[type="submit"]');

    let currentPage = 1;

    const filtersURL = getFiltersFromURL();
    applyFiltersToUI(filtersURL);
    fetchProducts(filtersURL);

    function renderProductCard(product) {
        return `
        <div class="product-card">
            <a href="/Shop/ProductDetails/${product.Id}">
                <div class="product-card-image">
                    <img src="${product.ImgUrl}" alt="${product.Name}" />
                </div>
            </a>
            <div class="product-card-details">
                <h3>${product.Name}</h3>
                <div class="price-container">
                    <p class="price">$${product.Price}</p>
                </div>
                <p>Type: ${product.Category}</p>
                <div class="info">
                    ${product.Quantity === 0 ? '<p class="out-of-stock">Out of stock</p>' : ''}
                </div>
            </div>
            <div class="product-card-buttons">
                <button class="cart-btn">Add To Cart</button>
                <button class="wishlist-btn" data-id="${product.Id}">
                   <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 1024 1024" height="30" width="30" xmlns="http://www.w3.org/2000/svg"><path d="M923 283.6a260.04 260.04 0 0 0-56.9-82.8 264.4 264.4 0 0 0-84-55.5A265.34 265.34 0 0 0 679.7 125c-49.3 0-97.4 13.5-139.2 39-10 6.1-19.5 12.8-28.5 20.1-9-7.3-18.5-14-28.5-20.1-41.8-25.5-89.9-39-139.2-39-35.5 0-69.9 6.8-102.4 20.3-31.4 13-59.7 31.7-84 55.5a258.44 258.44 0 0 0-56.9 82.8c-13.9 32.3-21 66.6-21 101.9 0 33.3 6.8 68 20.3 103.3 11.3 29.5 27.5 60.1 48.2 91 32.8 48.9 77.9 99.9 133.9 151.6 92.8 85.7 184.7 144.9 188.6 147.3l23.7 15.2c10.5 6.7 24 6.7 34.5 0l23.7-15.2c3.9-2.5 95.7-61.6 188.6-147.3 56-51.7 101.1-102.7 133.9-151.6 20.7-30.9 37-61.5 48.2-91 13.5-35.3 20.3-70 20.3-103.3.1-35.3-7-69.6-20.9-101.9zM512 814.8S156 586.7 156 385.5C156 283.6 240.3 201 344.3 201c73.1 0 136.5 40.8 167.7 100.4C543.2 241.8 606.6 201 679.7 201c104 0 188.3 82.6 188.3 184.5 0 201.2-356 429.3-356 429.3z"></path></svg>
                   </button>
               ${isUserInRoleAdmin ? `<button class="edit-btn" onclick="window.location.href='/Shop/EditProduct/${product.Id}'">Edit</button>` : ''}
            </div>
        </div>
    `;
    }

    function fetchProducts(filters, page = 1) {
        filters.CurrentPage = page;

        $.ajax({
            url: '/Shop/AllProductsFiltered',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(filters),
            beforeSend: () => { $loadingMessage.css('display', 'flex'); },
            complete: () => { $loadingMessage.css('display', 'none'); },
            success: function (response) {
                $productCardContainer.children().not($noProductMessage).remove();

                $paginationWrapper.show();
                $noProductMessage.css('display', 'none');
                if (!response.products || response.products.length === 0) {
                    $paginationWrapper.hide();
                    $noProductMessage.css('display', 'flex');
                }

                response.products.forEach(product => {
                    //here?
                    $productCardContainer.append(renderProductCard(product));
                });

                setupWishlistButtons();
                renderPagination(response.totalPages, response.currentPage);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }

    function renderPagination(totalPages, currentPage) {
        //console.log("Rendering pagination with:", { totalPages, currentPage });

        $paginationList.empty();

        const prevPage = currentPage > 1 ? currentPage - 1 : 1;
        const nextPage = currentPage < totalPages ? currentPage + 1 : totalPages;

        //console.log("Prev Page:", prevPage, "Next Page:", nextPage);

        // First page button
        $paginationList.append(`
            <li>
                <a class="first" data-page="1">
                    <svg width="20" height="20" viewBox="0 -5 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path d="M17 18l-6-6 6-6M11 18l-6-6 6-6" stroke="currentColor" stroke-width="1.3" fill="none" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                </a>
            </li>
        `);

        // Previous page button
        $paginationList.append(`
            <li>
                <a class="prev" data-page="${prevPage}">
                    <svg width="20" height="20" viewBox="0 -5 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path d="M15 18l-6-6 6-6" stroke="currentColor" stroke-width="1.3" fill="none" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                </a>
            </li>
        `);

        // Available pages
        const startPage = Math.max(1, currentPage - 1);
        const endPage = Math.min(totalPages, currentPage + 1);
        for (let i = startPage; i <= endPage; i++) {
            const activeClass = i === currentPage ? 'active' : '';
            $paginationList.append(`
                <li>
                    <a class="${activeClass}" data-page="${i}">${i}</a>
                </li>
            `);
        }

        // Next page button
        $paginationList.append(`
            <li>
                <a class="next" data-page="${nextPage}">
                    <svg width="20" height="20" viewBox="0 -5 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path d="M9 18l6-6-6-6" stroke="currentColor" stroke-width="1.3" fill="none" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                </a>
            </li>
        `);

        // Last page button
        $paginationList.append(`
            <li>
                <a class="last" data-page="${totalPages}">
                    <svg width="20" height="20" viewBox="0 -5 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path d="M7 18l6-6-6-6M13 18l6-6-6-6" stroke="currentColor" stroke-width="1.3" fill="none" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                </a>
            </li>
        `);
    }

    function getFilters() {
        return {
            SearchInput: $searchInput.val(),
            PriceRanges: $priceFilters.filter(':checked').map(function () { return this.value; }).get(),
            Categories: $categoryFilters.filter(':checked').map(function () { return this.value; }).get(),
            InStock: $inStockCheckbox.prop('checked'),
            SortByPrice: $sortPriceFilter.filter(':checked').val()
        };
    }

    function buildQueryString(filters) {
        const params = new URLSearchParams();

        if (filters.SearchInput) {
            params.append('input', filters.SearchInput);
        }

        filters.PriceRanges.forEach(priceRange => {
            params.append('priceRange', priceRange);
        });

        filters.Categories.forEach(category => {
            params.append('category', category);
        });

        if (filters.InStock) {
            params.append('inStock', 'true');
        }

        if (filters.SortByPrice) {
            params.append('sortByPrice', filters.SortByPrice);
        }

        params.append('pg', currentPage);

        return params.toString();
    }

    function getFiltersFromURL() {
        const params = new URLSearchParams(window.location.search);

        return {
            SearchInput: params.get('input') || '',
            PriceRanges: params.getAll('priceRange'),
            Categories: params.getAll('category'),
            InStock: params.get('inStock') === 'true',
            SortByPrice: params.get('sortByPrice') || '',
            pg: parseInt(params.get('pg'), 10) || 1
        };
    }

    function applyFiltersToUI(filters) {
        $searchInput.val(filters.SearchInput);

        $priceFilters.each(function () {
            $(this).prop('checked', filters.PriceRanges.includes($(this).val()));
        });

        $categoryFilters.each(function () {
            $(this).prop('checked', filters.Categories.includes($(this).val()));
        });

        $inStockCheckbox.prop('checked', filters.InStock);

        if (filters.SortByPrice) {
            $sortPriceFilter.val(filters.SortByPrice);
        }
    }

    function searchProducts() {
        const filters = getFilters();

        const queryString = buildQueryString(filters);
        window.history.pushState(null, null, `/Shop?${queryString}`);

        currentPage = 1;
        fetchProducts(filters, currentPage);
    }

    const isShopPage = window.location.pathname.includes('/Shop');
    if (isShopPage) {
        $searchInput.on('input', searchProducts);
    } else {
        $searchBtn.on('click keydown', searchProducts);
    }

    $filterTypesContainer.on('change', searchProducts);

    $paginationWrapper.on('click', 'a', function (event) {
        event.preventDefault();
        const page = $(this).data('page');

        if (page == currentPage) return;

        const filters = getFilters();

        currentPage = page;
        fetchProducts(filters, currentPage);
    });

    $resetFilterBtn.on('click', function (event) {
        event.preventDefault();

        $searchInput.val('');
        $priceFilters.prop('checked', false);
        $categoryFilters.prop('checked', false);
        $inStockCheckbox.prop('checked', false);
        $sortPriceFilter.prop('checked', false);

        const defaultFilters = {
            SearchInput: '',
            PriceRanges: [],
            Categories: [],
            InStock: false,
            SortByPrice: '',
            pg: 1
        };

        const queryString = buildQueryString(defaultFilters);
        window.history.pushState(null, null, `/Shop?${queryString}`);

        currentPage = 1;
        fetchProducts(defaultFilters, currentPage);
    });
});