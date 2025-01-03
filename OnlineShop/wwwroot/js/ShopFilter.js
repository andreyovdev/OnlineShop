$(document).ready(function () {
    const $productCardContainer = $('.product-card-container');
    const $paginationWrapper = $('.pagination-wrapper');
    const $noProductMessage = $('.no-product-message');
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
            success: function (response) {
                $productCardContainer.children().not($noProductMessage).remove();

                $paginationWrapper.show();
                $noProductMessage.hide();
                if (!response.products || response.products.length === 0) {
                    $paginationWrapper.hide();
                    $noProductMessage.show();
                }

                response.products.forEach(product => {
                    $productCardContainer.append(renderProductCard(product));
                });

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