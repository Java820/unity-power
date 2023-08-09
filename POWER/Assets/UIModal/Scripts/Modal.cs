
    using UnityEngine;

    /// <summary>
    /// Base class for all modals
    /// </summary>
    public abstract class Modal : MonoBehaviour
    {
        /// <summary>
        /// Closes this modal
        /// </summary>
        public virtual void Close()
        {
            GameObject joyStick = GameObject.Find("Floating Joystick");
            joyStick.GetComponent<Joystick>().stopDrag = false;

            Destroy(gameObject);
        }

        /// <summary>
        /// Shows the modal with the given content
        /// </summary>
        /// <param name="modalContent">Content to show</param>
        /// <param name="modalButton">Button properties</param>
        public abstract void Show(ModalContentBase modalContent, ModalButton[] modalButton);
        
    }
