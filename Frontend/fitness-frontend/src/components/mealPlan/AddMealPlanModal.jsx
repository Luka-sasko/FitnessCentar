import React, { useState } from "react";
import baseApi from "../../api/BaseApi";

const AddMealPlanModal = ({ open, onClose, onAdded }) => {
    const [name, setName] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        try {
            await baseApi.post("/mealplan", { Name: name });
            setLoading(false);
            setName("");
            onAdded();
            onClose();
        } catch (err) {
            setError("Error creating meal plan.");
            setLoading(false);
        }
    };

    if (!open) return null;
    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <button className="modal-close" onClick={onClose}>&times;</button>
                <h3 className="modal-title">ADD MEAL PLAN</h3>
                <form onSubmit={handleSubmit} className="modal-form">
                    <label>
                        Name:
                        <input
                            type="text"
                            required
                            value={name}
                            onChange={e => setName(e.target.value)}
                            placeholder="e.g. MY NEW PLAN"
                        />
                    </label>
                    {error && <div className="modal-error">{error}</div>}
                    <div className="modal-actions">
                        <button type="submit" disabled={loading}>
                            {loading ? "SAVING..." : "SAVE"}
                        </button>
                        <button type="button" className="modal-cancel" onClick={onClose}>
                            CANCEL
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default AddMealPlanModal;
