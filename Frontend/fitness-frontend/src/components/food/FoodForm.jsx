import React, { useState, useEffect } from "react";
import MealService from "../../api/services/MealService";

const FoodForm = ({ onSubmit, onClose }) => {
    const [name, setName] = useState("");
    const [weight, setWeight] = useState("");
    const [mealId, setMealId] = useState("");
    const [meals, setMeals] = useState([]);

    useEffect(() => {
        const fetchMeals = async () => {
            try {
                const response = await MealService.getByUser();
                console.log("Meal response:", response.data);
                setMeals(response.data.Items);
            } catch (error) {
                console.error("Failed to fetch meals", error);
            }
        };
        fetchMeals();
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit({
            name,
            weight: parseFloat(weight),
            mealId,
        });
        setName("");
        setWeight("");
        setMealId("");
    };

    const formContainerStyle = {
        background: "#ffffff",
        padding: "24px",
        borderRadius: "8px",
        boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)",
        marginTop: "20px",
        maxWidth: "400px",
        marginLeft: "auto",
        marginRight: "auto",
    };

    const formStyle = {
        display: "flex",
        flexDirection: "column",
        gap: "16px",
    };

    const inputGroupStyle = {
        display: "flex",
        flexDirection: "column",
        gap: "8px",
    };

    const labelStyle = {
        fontSize: "14px",
        fontWeight: "600",
        color: "#333",
    };

    const inputStyle = {
        padding: "10px",
        border: "1px solid #ccc",
        borderRadius: "4px",
        fontSize: "14px",
        transition: "border-color 0.3s ease",
    };

    const selectStyle = {
        ...inputStyle,
        appearance: "none",
        background: `url("data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='10' height='10' fill='%23333'><path d='M0 3l5 5 5-5H0z'/></svg>") no-repeat right 10px center`,
        paddingRight: "30px",
    };

    const buttonGroupStyle = {
        display: "flex",
        gap: "12px",
    };

    const buttonStyle = {
        padding: "10px 16px",
        border: "none",
        borderRadius: "4px",
        fontSize: "14px",
        fontWeight: "600",
        cursor: "pointer",
        transition: "background-color 0.3s ease, transform 0.2s ease",
    };

    const submitButtonStyle = {
        ...buttonStyle,
        backgroundColor: "#28a745",
        color: "#ffffff",
    };

    const cancelButtonStyle = {
        ...buttonStyle,
        backgroundColor: "#dc3545",
        color: "#ffffff",
    };

    return (
        <div style={formContainerStyle}>
            <h3 style={{ marginBottom: "16px", fontSize: "18px", color: "#333" }}>Dodaj hranu</h3>
            <form onSubmit={handleSubmit} style={formStyle}>
                <div style={inputGroupStyle}>
                    <label style={labelStyle}>Naziv:</label>
                    <input
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                        style={inputStyle}
                        onFocus={(e) => (e.target.style.borderColor = "#007bff")}
                        onBlur={(e) => (e.target.style.borderColor = "#ccc")}
                    />
                </div>
                <div style={inputGroupStyle}>
                    <label style={labelStyle}>Količina (g):</label>
                    <input
                        type="number"
                        value={weight}
                        onChange={(e) => setWeight(e.target.value)}
                        required
                        style={inputStyle}
                        onFocus={(e) => (e.target.style.borderColor = "#007bff")}
                        onBlur={(e) => (e.target.style.borderColor = "#ccc")}
                    />
                </div>
                <div style={inputGroupStyle}>
                    <label style={labelStyle}>Meal:</label>
                    <select
                        value={mealId}
                        onChange={(e) => setMealId(e.target.value)}
                        required
                        style={selectStyle}
                        onFocus={(e) => (e.target.style.borderColor = "#007bff")}
                        onBlur={(e) => (e.target.style.borderColor = "#ccc")}
                    >
                        <option value="">-- Odaberi obrok --</option>
                        {meals.map((meal) => (
                            <option key={meal.Id} value={meal.Id}>
                                {meal.Name}
                            </option>
                        ))}
                    </select>
                </div>
                <div style={buttonGroupStyle}>
                    <button type="submit" style={submitButtonStyle}>
                        ✅ Spremi
                    </button>
                    <button
                        type="button"
                        onClick={onClose}
                        style={cancelButtonStyle}
                    >
                        ❌ Odustani
                    </button>
                </div>
            </form>
        </div>
    );
};

export default FoodForm;