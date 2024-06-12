using Microsoft.AspNetCore.Components;

namespace GigaComic.Client.Components.Modal
{
    public class BaseModal : ComponentBase
    {
        public virtual string InitialText { get; set; } = "";
        public virtual string ModalText { get; set; } = "";

        protected ConfirmationModal? _confirmationModal;
        protected bool _isError = false;

        public virtual void OnConfirm()
        {
            _confirmationModal?.Hide();
            StateHasChanged();
        }

        public virtual void Show()
        {
            _isError = false;
            this.ModalText = this.InitialText;
            _confirmationModal?.Show();
            StateHasChanged();
        }

        public virtual void Show(string message)
        {
            _isError = false;
            this.InitialText = message;
            this.ModalText = message;
            _confirmationModal?.Show();
            StateHasChanged();
        }

        public virtual void ShowError(string message)
        {
            _isError = true;
            this.ModalText = message;
            _confirmationModal?.Show();
            StateHasChanged();
        }
    }
}
