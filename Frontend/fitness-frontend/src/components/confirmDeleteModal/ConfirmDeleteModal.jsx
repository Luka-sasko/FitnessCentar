import React from "react";
import "../../App.css";

const ConfirmDeleteModal = ({ open, onClose, onConfirm, text }) => {
    if (!open) return null;
    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <h3 className="modal-title">Are you sure you want to delete?</h3>
                <p style={{ marginBottom: 24, textAlign: "center", color: "#555" }}>
                    {text}
                </p>
                <div className="modal-actions">
                    <button
                        onClick={onConfirm}
                        style={{
                            background: "#e03c3c",
                            color: "#fff",
                            padding: "8px 22px"
                        }}
                    >
                        Yes
                    </button>
                    <button
                        onClick={onClose}
                        className="modal-cancel"
                        style={{ padding: "8px 22px" }}
                    >
                        No
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ConfirmDeleteModal;
