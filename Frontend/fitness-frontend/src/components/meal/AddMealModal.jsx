import React, { useState } from "react";
import baseApi from "../../api/BaseApi";
import "../../App.css";

const AddMealModal = ({ open, onClose, mealPlanId, onAdded }) => {
    const [name, setName] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        try {
            const mealRes = await baseApi.post("/meal", { Name: name });
            const mealId = mealRes.data;

            await baseApi.post("/mealplanmeal", {
                MealPlanId: mealPlanId,
                MealId: mealId,
            });

            setLoading(false);
            setName("");
            onAdded();
            onClose();
        } catch (err) {
            setError("Error creating meal.");
            setLoading(false);
        }
    };

    if (!open) return null;

    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <button className="modal-close" onClick={onClose}>&times;</button>
                <h3 className="modal-title">ADD MEAL</h3>
                <form onSubmit={handleSubmit} className="modal-form">
                    <label>
                        Name:
                        <input
                            type="text"
                            required
                            value={name}
                            onChange={e => setName(e.target.value)}
                            placeholder="e.g. Lunch"
                        />
                    </label>
                    {error && <div className="modal-error">{error}</div>}
                    <div className="modal-actions">
                        <button
                            type="submit"
                            className="modal-cancel"
                            disabled={loading}
                            onMouseOver={(e) => e.currentTarget.style.backgroundColor = "#333"}
                            onMouseOut={(e) => e.currentTarget.style.backgroundColor = "#000"}
                        >
                            {loading ? "SAVING..." : "SAVE"}
                        </button>
                        <button
                            type="button"
                            className="modal-cancel"
                            onClick={onClose}
                            onMouseOver={(e) => e.currentTarget.style.backgroundColor = "#333"}
                            onMouseOut={(e) => e.currentTarget.style.backgroundColor = "#000"}
                        >
                            CANCEL
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default AddMealModal;
