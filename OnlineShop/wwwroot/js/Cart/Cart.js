$(document).ready(function () {
    const cartContainer = document.querySelector('.cart-container');
    const $cartCardContainer = $('.cart-products-container');
    const $cartAddressContainer = $('.cart-address-container');
    const $summary = $('.cart-price-container');
    const $loadingMessage = $('.loading-message');
    const $noProductMessage = $('.no-product-message');
    const $pageHeading = $('.page-heading');
    const $subTotalPrice = $('.subtotal-container span').eq(1);
    const $totalPrice = $('.total span').eq(1);

    let productsInCartCount = 0;

    let productPrices = {}; 

    fetchProducts();
    updateSummary();

    function renderCartCard(product) {
        return `
        <div class="cart-product-card" data-id="${product.ProductId}">
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
                    <button class="counter-btn minus-btn">-</button>
                    <span class="counter-number">${product.QuantitySelected}</span>
                    <button class="counter-btn plus-btn">+</button>
                </div>
            </div>
            <div class="remove-from-cart-btn">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="25" width="25" xmlns="http://www.w3.org/2000/svg">
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
            beforeSend: () => {
                $loadingMessage.css('display', 'flex');
            },
            complete: () => {
                $loadingMessage.css('display', 'none');
            },
            success: function (response) {
                $cartCardContainer.children().not($noProductMessage).remove();

                $noProductMessage.css('display', 'none');
                $pageHeading.css('display', 'flex');
                $summary.css('display', 'flex');
                $cartAddressContainer.css('display', 'flex');
                $cartCardContainer.css('flex-grow', '1');
                cartContainer.classList.add('cart-container-dynamic');

                if (!response.products || response.products.length === 0) {
                    $noProductMessage.css('display', 'flex');
                    $cartAddressContainer.css('display', 'none');
                    $pageHeading.css('display', 'none');
                    $summary.css('display', 'none');
                    $cartCardContainer.css('flex-grow', '0');
                    cartContainer.classList.remove('cart-container-dynamic');
                }
                

                response.products.forEach(product => {
                    $cartCardContainer.append(renderCartCard(product));
                    productPrices[product.ProductId] = { price: 0, quantity: 0 }; 
                    productPrices[product.ProductId].price = product.Price;
                    productPrices[product.ProductId].quantity = product.QuantitySelected;
                });
                setupRemoveFromCartButtons();
                setupProductQuantityButtons();
                updateSummary();
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
                        $cartAddressContainer.css('display', 'none');
                        $pageHeading.css('display', 'none');
                        $summary.css('display', 'none');
                        $cartCardContainer.css('flex-grow', '0');
                        cartContainer.classList.remove('cart-container-dynamic');
                    }

                    delete productPrices[productId];
                    updateSummary();
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        });
    }

    function setupProductQuantityButtons() {
        $('.plus-btn').on('click', function () {
            updateCartQuantity($(this), 1);
        });

        $('.minus-btn').on('click', function () {
            updateCartQuantity($(this), -1);
        });
    }

    function updateCartQuantity(buttonElement, quantity) {
        const productId = buttonElement.closest('.cart-product-card').data('id');
        const quantityNumber = buttonElement.closest('.count-btn-container').find('.counter-number');

        if (!isUserAuthenticated) {
            console.log("unauthorized");
            return;
        }

            $.ajax({
                url: '/Cart/UpdateCart',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ ProductId: productId, Quantity: quantity }), 
                success: function (response) {
                    if (response === -1) { return; }
                   
                    quantityNumber.text(response);
                    productPrices[productId].quantity = response;
                    updateSummary();
                    
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        

    }

    function updateSummary() {
        let subTotalPrice = Object.values(productPrices).reduce((sum, product) => sum + product.price * product.quantity, 0);
        let totalPrice = 0;

        $subTotalPrice.text('$' + subTotalPrice.toFixed(2));
        $totalPrice.text($subTotalPrice.text());
    }
});
