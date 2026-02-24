import "./teamModal.css";

const TeamsModal = ({ title, onClose, children }) => {
  return (
    <div className="modal-overlay">
      <div className="modal-content">

        <button className="close-button" onClick={onClose}>
          ✕
        </button>

        {title && <h2>{title}</h2>}

        {children}

      </div>
    </div>
  );
};

export default TeamsModal;