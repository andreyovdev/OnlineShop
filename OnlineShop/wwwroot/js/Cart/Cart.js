$(document).ready(function () {
    const $cartCardContainer = $('.cart-products-container');
    const $cartContainer = $('.cart-container');
    const $summary = $('.cart-price-container');
    const $loadingMessage = $('.loading-message');
    const $noProductMessage = $('.no-product-message');
    const $pageHeading = $('.page-heading');

    let productsInCartCount = 0;

    fetchProducts();

    function renderWishlistCard(product) {
        return `
        <div class="cart-product-card" data-id="${product.Id}">
            <div>
                <img class="cart-img" src="${product.ImgUrl}" alt="${product.Name}">
            </div>
            <div class="product-description">
                <h3>${product.Name}</h3>
                <p>Price: $${product.Price}</p>
                <p>Category: ${product.Category}</p>
            </div>
            <div class="button-section">
                <div class="count-btn-container">
                    <button class="counter-btn" onclick="decreaseQuantity(${product.Id})">-</button>
                    <span>${product.Quantity}</span>
                    <button class="counter-btn" onclick="increaseQuantity(${product.Id})">+</button>
                </div>
            </div>
            <div class="remove-from-cart-btn">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="25" width="25" xmlns="http://www.w3.org/2000/svg" onclick="removeFromWishlist(${product.Id})">
                    <path fill="none" d="M0 0h24v24H0z"></path>
                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"></path>
                </svg>
            </div>
        </div>
    `;
    }

   

    function fetchProducts() {

        if (!isUserAuthenticated) {
            console.log("not authed")
            $noProductMessage.css('display', 'flex');
            return;
        }

        $.ajax({
            url: '/Cart/AllProductsInCart',
            type: 'POST',
            contentType: 'application/json',
            beforeSend: () => { $loadingMessage.css('display', 'flex'); },
            complete: () => { $loadingMessage.css('display', 'none'); },
            success: function (response) {
                $cartCardContainer.children().not($noProductMessage).remove();

                $noProductMessage.css('display', 'none');
                $pageHeading.css('display', 'flex');
                $summary.css('display', 'flex');
                $cartContainer.css('flex-direction', 'row');
                $cartCardContainer.css('flex-grow', '1');
                if (!response.products || response.products.length === 0) {
                    $noProductMessage.css('display', 'flex');
                    $pageHeading.css('display', 'none');
                    $summary.css('display', 'none');
                    $cartContainer.css('flex-direction', 'column');
                    $cartCardContainer.css('flex-grow', '0');

                }
                

                response.products.forEach(product => {
                    $cartCardContainer.append(renderWishlistCard(product));
                });
                setupRemoveFromCartButtons();
                productsInCartCount = response.products.length;
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }

    function setupRemoveFromCartButtons() {
        $('.remove-from-cart-btn').on('click', function () {
            const buttonElement = $(this);
            const productId = buttonElement.closest('.cart-product-card').data('id');

            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productId),
                success: function () {
                    const cardElement = buttonElement.closest(`.cart-product-card`).remove();

                    productsInCartCount--;

                    if (productsInCartCount <= 0) {
                        $noProductMessage.css('display', 'flex');
                        $pageHeading.css('display', 'none');
                        $summary.css('display', 'none');
                        $cartContainer.css('flex-direction', 'column');
                        $cartCardContainer.css('flex-grow', '0');
                    }

                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        });
    }

});
