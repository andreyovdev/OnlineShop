$(document).ready(function () {
    const $wishlistCardContainer = $('.wishlist-products-container');
    const $loadingMessage = $('.loading-message');
    const $noProductMessage = $('.no-product-message');
    const $pageHeading = $('.page-heading');

    let productsInWishlistCount = 0;

    fetchProducts();

    function renderWishlistCard(product) {

        let isInCart = false;

        if (isUserAuthenticated) {
            isInCart = isProductInCart(product.Id);
        }

        return `
            <div class="wishlist-card" data-id="${product.Id}">
                <div>
                    <img class="img-container" alt="${product.Name}" src="${product.ImgUrl}">
                </div>
                <div class="product-card-details">
                    <h3>${product.Name}</h3>
                    <div class="price-container">
                        <p class="price">$${product.Price}</p>
                    </div>
                    <p>Type: ${product.Category}</p>
                    <div class="info"></div>
                </div>
                <div class="wishlist-btn-container">
                    <button class="cart-wishlist-btn">
                        <a class="cart-add" style="display: ${isInCart ? 'none' : 'inline-block'};">Add To Cart</a>
                        <a class="cart-added" href="/Cart" style="display: ${isInCart ? 'inline-block' : 'none'};">View In Cart</a>
                     </button>
                    <button class="remove-from-wishlist-btn">Remove from Wishlist</button>
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
            url: '/Wishlist/AllProductsInWishlist',
            type: 'POST',
            contentType: 'application/json',
            beforeSend: () => { $loadingMessage.css('display', 'flex'); },
            complete: () => { $loadingMessage.css('display', 'none'); },
            success: function (response) {
                $wishlistCardContainer.children().not($noProductMessage).remove();

                $noProductMessage.css('display', 'none');
                $pageHeading.css('display', 'flex');
                if (!response.products || response.products.length === 0) {
                    $noProductMessage.css('display', 'flex');
                    $pageHeading.css('display', 'none');
                }
                

                response.products.forEach(product => {
                    $wishlistCardContainer.append(renderWishlistCard(product));
                });
                setupWishlistButtons();
                setupCartButtons();

                productsInWishlistCount = response.products.length;


            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }

    function setupWishlistButtons() {
        $('.remove-from-wishlist-btn').on('click', function () {
            const buttonElement = $(this);
            const productId = buttonElement.closest('.wishlist-card').data('id'); 

            $.ajax({
                url: '/Wishlist/RemoveFromWishlist',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productId),
                success: function () {
                    const cardElement = buttonElement.closest(`.wishlist-card`).remove();

                    productsInWishlistCount--;

                   

                    $noProductMessage.css('display', 'none');
                    $pageHeading.css('display', 'flex');
                    if (productsInWishlistCount <= 0) {
                        $noProductMessage.css('display', 'flex');
                        $pageHeading.css('display', 'none');
                    }

                    //Update products in wishlist number on navigation bar
                    wishlistCount--;
                    wishlistCountElement.textContent = `${wishlistCount}`
                    if (wishlistCount === 0) {
                        wishlistCountElement.style.display = 'none';
                    } else {
                        wishlistCountElement.style.display = 'block';
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        });
    }

    function setupCartButtons() {
        $('.cart-wishlist-btn').on('click', function () {
            const buttonElement = $(this).closest(".wishlist-card");
            const productId = buttonElement.data('id');
            const cartAddText = buttonElement.find('.cart-add');
            const cartAddedText = buttonElement.find('.cart-added');

            if (!isUserAuthenticated) {
                window.location.href = "/Identity/Account/Login";
            }

            if (isProductInCart(productId)) {
                console.log("already added")
                return;
            }

            $.ajax({
                url: '/Cart/AddToCart',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productId),
                success: function (response) {
                    if (response === false) {
                        console.log("sorry you cant add this product to cart");
                        return;
                    }
                    if (cartAddText.css('display') === 'none') {
                        cartAddText.css('display', 'inline-block');
                        cartAddedText.css('display', 'none');
                    } else {
                        cartAddText.css('display', 'none');
                        cartAddedText.css('display', 'inline-block');
                    }

                    //Update products in cart number on navigation bar
                    cartCount++;
                    cartCountElement.textContent = `${cartCount}`
                    if (cartCount === 0) {
                        cartCountElement.style.display = 'none';
                    } else {
                        cartCountElement.style.display = 'block';
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        });
    }
});
