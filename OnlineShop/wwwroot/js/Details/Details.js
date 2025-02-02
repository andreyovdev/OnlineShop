$(document).ready(function () {
    const $addToWishlistBtn = $('.add-to-wishlist-btn');
    const $addToCartBtn = $('.add-to-cart-btn');

    renderProductDetails();

    function renderProductDetails() {
        let isWishlisted = false;
        let isInCart = false;

        if (isUserAuthenticated) {
            isWishlisted = isProductInWishlist(productId);
            isInCart = isProductInCart(productId);
        }

        if (isWishlisted) {
            $addToWishlistBtn.text('Remove from Wishlist');
        } else {
            $addToWishlistBtn.text('Add to Wishlist');
        }

        if (isInCart) {
            $addToCartBtn.text('Remove from Cart');
        } else {
            $addToCartBtn.text('Add to Cart');
        }

        setupWishlistButton();
        setupCartButton();
    }

    function setupWishlistButton() {

        $addToWishlistBtn.on('click', function () {
            const buttonElement = $(this);

            if (!isUserAuthenticated) {
                window.location.href = "/Identity/Account/Login";
            }

            $.ajax({
                url: isProductInWishlist(productId) ? '/Wishlist/RemoveFromWishlist' : '/Wishlist/AddToWishlist',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productId),
                success: function () {
                    if (buttonElement.text().trim() === 'Add to Wishlist') {
                        buttonElement.text('Remove from Wishlist');

                        wishlistCount++;
                    } else {
                        buttonElement.text('Add to Wishlist');

                        wishlistCount--;
                    }

                    //Update products in wishlist number on navigation bar
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

    function setupCartButton() {
        $addToCartBtn.on('click', function () {
            const buttonElement = $(this);

            if (!isUserAuthenticated) {
                window.location.href = "/Identity/Account/Login";
            }

            $.ajax({
                url: isProductInCart(productId) ? '/Cart/RemoveFromCart' : '/Cart/AddToCart',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productId),
                success: function () {

                    if (buttonElement.text().trim() === 'Add to Cart') {
                        buttonElement.text('Remove from Cart');

                        cartCount++;
                    } else {
                        buttonElement.text('Add to Cart');

                        cartCount--;
                    }

                    //Update products in cart number on navigation bar
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
