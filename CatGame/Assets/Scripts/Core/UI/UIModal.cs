using System;

namespace Core
{
    public abstract class UIModal : UIBase
    {
        public event Action Closing;
        public event Action Closed;

        protected override void OnEnable()
        {
            base.OnEnable();

            OnOpen();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            OnClose();
        }

        protected virtual void OnOpen()
        {
            UIManager.Instance.PushModal(this);
        }

        protected virtual void OnClose()
        {
            UIManager.Instance.PopModal(this);
        }

        public void Open()
        {
            if (gameObject.activeSelf)
                return;

            gameObject.SetActive(true);
        }

        public void Close()
        {
            if (!gameObject.activeSelf)
                return;

            if (Closing != null)
                Closing();

            gameObject.SetActive(false);

            if (Closed != null)
                Closed();
        }
    }
}