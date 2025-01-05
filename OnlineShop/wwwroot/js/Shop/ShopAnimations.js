$(document).ready(function () {
    const $document = $(document);

    // Filter Button Toggle for Mobile
    $document.on('click', '.close-tab', function () {
        const filterContainer = document.querySelector('.filter-container');
        const filterHeader = document.querySelector('.filter-header');
        const filterTypeContainer = document.querySelector('.filter-types-container');
        const resetBtn = document.querySelector('.reset-btn-hide');
        const crossSvg = document.querySelector('.cross-svg');
        const listSvg = document.querySelector('.list-svg');

        if (window.innerWidth < 768) {
            // Toggle the open/closed classes
            filterContainer.classList.toggle('filter-container-mobile-open');
            filterContainer.classList.toggle('filter-container-mobile-closed');

            filterHeader.classList.toggle('filter-header-mobile-open');
            filterHeader.classList.toggle('filter-header-mobile-closed');

            filterTypeContainer.classList.toggle('filter-types-container-mobile');

            resetBtn.classList.toggle('reset-btn');
            resetBtn.classList.toggle('reset-btn-hide');

            // Toggle icons
            if (filterContainer.classList.contains('filter-container-mobile-open')) {
                crossSvg.style.display = 'inline';
                listSvg.style.display = 'none';
            } else {
                crossSvg.style.display = 'none';
                listSvg.style.display = 'inline';
            }
        }
    });

    // Product Card Tilt and Image Hover Effects
    $document.on('mousemove', '.product-card', function (e) {
        const card = e.currentTarget;
        const { width, height, left, top } = card.getBoundingClientRect();
        const x = e.clientX - left - width / 2;
        const y = e.clientY - top - height / 2;

        const rotateX = -(y / height) * 15;
        const rotateY = (x / width) * 15;

        card.style.transition = 'transform 0.1s ease';
        card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
    });

    $document.on('mouseleave', '.product-card', function (e) {
        const card = e.currentTarget;
        card.style.transition = 'transform 0.3s ease';
        card.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg)';
    });

    $document.on('mouseenter', '.product-card-image img', function (e) {
        const image = e.currentTarget;
        image.style.transform = 'scale(1.2)';
    });

    $document.on('mouseleave', '.product-card-image img', function (e) {
        const image = e.currentTarget;
        image.style.transform = 'scale(1)';
    });
});
