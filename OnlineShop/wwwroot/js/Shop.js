const productCards = document.querySelectorAll('.product-card');

productCards.forEach((card) => {
    const imageContainer = card.querySelector('.animated-product-item img');

    card.addEventListener('mousemove', (e) => {
        const { width, height, left, top } = card.getBoundingClientRect();
        const x = e.clientX - left - width / 2;
        const y = e.clientY - top - height / 2;

        const rotateX = -(y / height) * 20;
        const rotateY = (x / width) * 20;

        card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
    });

    imageContainer.addEventListener('mouseenter', () => {
        imageContainer.style.transform = 'scale3d(1.2, 1.2, 1.2)';
    });

    imageContainer.addEventListener('mouseleave', () => {
        imageContainer.style.transform = 'scale3d(1, 1, 1)';
    });

    card.addEventListener('mouseleave', () => {
        card.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg)';
        imageContainer.style.transform = 'scale3d(1, 1, 1)'; 
    });
});




const filterButton = document.querySelector('.close-tab');
const crossSvg = document.querySelector('.cross-svg');
const listSvg = document.querySelector('.list-svg');
const filterContainer = document.querySelector('.filter-container');
const filterHeader = document.querySelector('.filter-header');
const filterTypeContainer = document.querySelector('.filter-types-container');
const resetBtn = document.querySelector('.reset-btn-hide');

filterButton.addEventListener('click', () => {
  if (window.innerWidth < 768) {
    
    if(filterContainer.classList.contains('filter-container-mobile-open')) {
      filterContainer.classList.add('filter-container-mobile-closed');
      filterContainer.classList.remove('filter-container-mobile-open');

      filterHeader.classList.add('filter-header-mobile-closed');
      filterHeader.classList.remove('filter-header-mobile-open');

      filterTypeContainer.classList.remove('filter-types-container-mobile');

      resetBtn.classList.remove('reset-btn');
      resetBtn.classList.add('reset-btn-hide');

      crossSvg.style='display:none;';
      listSvg.style='display:inline;';
    } else {
      filterContainer.classList.add('filter-container-mobile-open');
      filterContainer.classList.remove('filter-container-mobile-closed');

      filterHeader.classList.add('filter-header-mobile-open');
      filterHeader.classList.remove('filter-header-mobile-closed');

      filterTypeContainer.classList.add('filter-types-container-mobile');

      resetBtn.classList.add('reset-btn');
      resetBtn.classList.remove('reset-btn-hide');

      crossSvg.style='display:inline;';
      listSvg.style='display:none;';
    }

  }
});
