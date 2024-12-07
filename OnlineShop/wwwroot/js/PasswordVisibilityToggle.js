const passwordInput = document.querySelector('.password');
const showPassSvgContainer = document.querySelector('.show-pass-svg-container');
const showPassSvg = document.querySelector('.show-pass');
const hidePassSvg = document.querySelector('.hide-pass');

showPassSvgContainer.addEventListener('click', () => {
    if(passwordInput.type=='text') {
        passwordInput.type='password';
        showPassSvg.style.display = 'flex';
        hidePassSvg.style.display = 'none';
    } else {
        passwordInput.type='text';
        showPassSvg.style.display = 'none';
        hidePassSvg.style.display = 'flex';
    }
});