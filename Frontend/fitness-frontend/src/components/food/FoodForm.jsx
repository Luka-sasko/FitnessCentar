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

    return (
        <div style={{
            background: "#fff",
            padding: "24px",
            borderRadius: "8px",
            boxShadow: "0 4px 6px rgba(0,0,0,0.1)",
            marginTop: "20px",
            maxWidth: "500px",
            marginInline: "auto"
        }}>
            <h3 style={{ fontSize: "20px", color: "#000", marginBottom: "16px" }}>Add Food</h3>
            <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                <label style={{ fontWeight: 600, color: "#000" }}>
                    Name:
                    <input
                        type="text"
                        required
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        style={{
                            padding: "10px",
                            border: "1px solid #ccc",
                            borderRadius: "6px"
                        }}
                    />
                </label>
                <label style={{ fontWeight: 600, color: "#000" }}>
                    Weight (g):
                    <input
                        type="number"
                        required
                        value={weight}
                        onChange={(e) => setWeight(e.target.value)}
                        style={{
                            padding: "10px",
                            border: "1px solid #ccc",
                            borderRadius: "6px"
                        }}
                    />
                </label>
                <label style={{ fontWeight: 600, color: "#000" }}>
                    Meal:
                    <select
                        required
                        value={mealId}
                        onChange={(e) => setMealId(e.target.value)}
                        style={{
                            padding: "10px",
                            border: "1px solid #ccc",
                            borderRadius: "6px"
                        }}
                    >
                        <option value="">-- Select Meal --</option>
                        {meals.map(meal => (
                            <option key={meal.Id} value={meal.Id}>{meal.Name}</option>
                        ))}
                    </select>
                </label>
                <div style={{ display: "flex", gap: "12px", justifyContent: "flex-end" }}>
                    <button
                        type="submit"
                        style={{
                            background: "#000",
                            color: "#fff",
                            border: "none",
                            borderRadius: "6px",
                            padding: "10px 16px",
                            cursor: "pointer"
                        }}
                        onMouseOver={(e) => e.currentTarget.style.background = "#333"}
                        onMouseOut={(e) => e.currentTarget.style.background = "#000"}
                    >
                        ✅ Save
                    </button>
                    <button
                        type="button"
                        onClick={onClose}
                        style={{
                            background: "#000",
                            color: "#fff",
                            border: "none",
                            borderRadius: "6px",
                            padding: "10px 16px",
                            cursor: "pointer"
                        }}
                        onMouseOver={(e) => e.currentTarget.style.background = "#333"}
                        onMouseOut={(e) => e.currentTarget.style.background = "#000"}
                    >
                        ❌ Cancel
                    </button>
                </div>
            </form>
        </div>
    );
};

export default FoodForm;
