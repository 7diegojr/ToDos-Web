const userMenu = document.getElementById('userMenu');
const dropdown = document.getElementById('dropdown');

/* ao clicar no menu do usuário, a classe active é
alternada no dropdown com classList.toggle() */
userMenu.addEventListener('click', (e) => {
    e.stopPropagation(); /* impede que o clique se propague para o document */
    dropdown.classList.toggle('active');
});

document.addEventListener('click', () => {
    /* Qualquer clique fora do menu remove a classe active, fechando o dropdown. */
    dropdown.classList.remove('active');
});