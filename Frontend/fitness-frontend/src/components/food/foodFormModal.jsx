import React, { useState } from "react";
import baseApi from "../../api/BaseApi";
import "../../../src/styles/FoodFormModal.css";

const FoodFormModal = ({ mealId, onClose, onFoodAdded }) => {
    const [name, setName] = useState("");
    const [weight, setWeight] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        try {
            await baseApi.post("/food", {
                MealId: mealId,
                Name: name,
                Weight: parseFloat(weight)
            });
            setLoading(false);
            onFoodAdded();
        } catch (err) {
            setError("Error");
            setLoading(false);
        }
    };

    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <button className="modal-close" onClick={onClose}>&times;</button>
                <h3 className="modal-title">ADD FOOD</h3>
                <form onSubmit={handleSubmit} className="modal-form">
                    <label>
                        NAME:
                        <input
                            type="text"
                            required
                            value={name}
                            onChange={e => setName(e.target.value)}
                            placeholder="e.g. Banana"
                        />
                    </label>
                    <label>
                        AMOUNT (g):
                        <input
                            type="number"
                            required
                            min={1}
                            value={weight}
                            onChange={e => setWeight(e.target.value)}
                            placeholder="e.g. 120"
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

export default FoodFormModal;
