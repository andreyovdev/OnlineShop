const elements = document.querySelectorAll('.video-card');

elements.forEach((element) => {
    element.addEventListener('mousemove', (e) => {
        const { width, height, left, top } = element.getBoundingClientRect();
        const x = e.clientX - left - width / 2; 
        const y = e.clientY - top - height / 2;
      
        const rotateX = -(y / height) * 20;
        const rotateY = (x / width) * 20;
      
        element.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale3d(1.05, 1.05, 1)`;
      });
      
      element.addEventListener('mouseleave', () => {
        element.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg) scale3d(1, 1, 1)';
      });
      
})
