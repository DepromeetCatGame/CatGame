namespace Core
{
    public abstract class UIQueuedModal : UIModal
    {
        public abstract ModalType ModalType { get; }

        protected override void OnClose()
        {
            base.OnClose();
            UIManager.Instance.DequeueModal(ModalType);
        }
    }
}