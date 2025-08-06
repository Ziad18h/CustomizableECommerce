document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('resendForm');
    var submitBtn = document.getElementById('submitBtn');
    var btnText = submitBtn ? submitBtn.querySelector('.btn-text') : null;

    if (form && submitBtn && btnText) {
        form.addEventListener('submit', function () {
            submitBtn.disabled = true;
            btnText.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Sending...';

            setTimeout(function () {
                if (submitBtn.disabled) {
                    submitBtn.disabled = false;
                    btnText.innerHTML = '<i class="bi bi-arrow-repeat me-2"></i>Resend Confirmation Email';
                }
            }, 10000);
        });
    }

    var emailInput = document.querySelector('input[name="EmailOrUserName"]');
    if (emailInput) {
        emailInput.addEventListener('input', function () {
            var value = this.value.trim();
            var hasAtSymbol = value.indexOf('@');
            var isEmail = hasAtSymbol > 0;

            if (value.length === 0) {
                this.classList.remove('is-valid', 'is-invalid');
            } else if (isEmail) {
                var emailParts = value.split('@');
                if (emailParts.length === 2 && emailParts[0].length > 0 && emailParts[1].indexOf('.') > 0) {
                    this.classList.remove('is-invalid');
                    this.classList.add('is-valid');
                } else {
                    this.classList.remove('is-valid');
                    this.classList.add('is-invalid');
                }
            } else {
                if (value.length >= 3) {
                    this.classList.remove('is-invalid');
                    this.classList.add('is-valid');
                } else {
                    this.classList.remove('is-valid');
                    this.classList.add('is-invalid');
                }
            }
        });
    }
});
