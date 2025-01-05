function setupWishlistButtons() {
    document.querySelectorAll('.wishlist-btn').forEach(button => {
        button.addEventListener('click', async (event) => {
            const buttonElement = event.target.closest('.wishlist-btn');
            const productId = buttonElement.getAttribute('data-id'); 

            const response = await fetch('/Shop/AddToWishlist', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(productId)
            });

            if (response.ok) {
                alert(productId);
            } else {
                alert('Failed to add product to wishlist.');
            }
        });
    });
}