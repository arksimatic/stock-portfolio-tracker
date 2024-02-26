document.querySelectorAll('.delete-btn').forEach(btn => {
    btn.addEventListener('click', function (e) {
        var walletName = this.getAttribute('data-wallet-name');
        if (!confirm(`Do you wish to delete wallet ${walletName}?`)) {
            e.preventDefault();
        }
    });
});