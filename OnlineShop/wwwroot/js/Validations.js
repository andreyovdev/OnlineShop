function isProductInWishlist(productId) {
    let isInWishlist = false;
    $.ajax({
        url: `/Wishlist/IsProductInWishlist?productId=${productId}`,
        type: 'GET',
        async: false,
        success: function (response) {
            isInWishlist = response;
        },
        error: function () {
            isInWishlist = false;
        }
    });
    return isInWishlist;
}

function isProductInCart(productId) {
    let isInCart = false;
    $.ajax({
        url: `/Cart/IsProductInCart?productId=${productId}`,
        type: 'GET',
        async: false,
        success: function (response) {
            isInCart = response;
        },
        error: function () {
            isInCart = false;
        }
    });
    return isInCart;
}