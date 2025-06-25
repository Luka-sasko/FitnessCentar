import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { mealPlanStore } from "../stores/MealPlanStore";
import GenericTable from "../components/Common/GenericTable";
import baseApi from "../api/BaseApi";
import FoodFormModal from "../components/food/foodFormModal";
import AddMealPlanModal from "../components/mealPlan/AddMealPlanModal";
import AddMealModal from "../components/meal/AddMealModal";
import ConfirmDeleteModal from "../components/confirmDeleteModal/ConfirmDeleteModal";
import "../App.css";

const deleteButtonStyle = {
  color: "#e03c3c",
  background: "none",
  border: "none",
  fontWeight: "bold",
  fontSize: "1.2em",
  cursor: "pointer",
  marginLeft: 8
};

const MealPlanPage = observer(() => {
  const [selectedMeals, setSelectedMeals] = useState([]);
  const [selectedMealFoods, setSelectedMealFoods] = useState([]);
  const [selectedMealId, setSelectedMealId] = useState(null);

  const [foodModalOpen, setFoodModalOpen] = useState(false);
  const [foodModalMealId, setFoodModalMealId] = useState(null);

  const [addModalOpen, setAddModalOpen] = useState(false);

  const [addMealModalOpen, setAddMealModalOpen] = useState(false);
  const [currentMealPlan, setCurrentMealPlan] = useState(null);

  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState({ type: null, id: null });

  useEffect(() => {
    mealPlanStore.fetchAll();
  }, []);


  const openDeleteModal = (type, id) => {
    setDeleteTarget({ type, id });
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirmed = async () => {
    setDeleteModalOpen(false);
    if (deleteTarget.type === "mealplan") {
      await handleDeleteMealPlan(deleteTarget.id);
    } else if (deleteTarget.type === "meal") {
      await handleDeleteMeal(deleteTarget.id);
    } else if (deleteTarget.type === "food") {
      await handleDeleteFood(deleteTarget.id);
    }
    setDeleteTarget({ type: null, id: null });
  };

  const handleDeleteMealPlan = async (id) => {
    await baseApi.delete(`/mealplan?id=${id}`);
    mealPlanStore.fetchAll();
    setSelectedMeals([]);
    setCurrentMealPlan(null);
  };

  const handleDeleteMeal = async (id) => {
    await baseApi.delete(`/meal?id=${id}`);
    if (currentMealPlan) await handleMealPlanClick(currentMealPlan);
  };

  const handleDeleteFood = async (id) => {
    await baseApi.delete(`/food?id=${id}`);
    if (selectedMealId) await handleMealClick({ Id: selectedMealId });
  };


  const handleMealPlanClick = async (mealPlan) => {
    setCurrentMealPlan(mealPlan);
    try {
      const response = await baseApi.getAll("/mealplanmeal", {
        mealPlanId: mealPlan.Id,
        pageNumber: 1,
        pageSize: 100,
      });
      const mealPlanMeals = response.data.Items || [];
      const mealIds = mealPlanMeals.map(mpm => mpm.mealId || mpm.MealId);

      const mealRequests = mealIds.map(id =>
        baseApi.get(`/meal/${id}`).then(res => res.data).catch(() => null)
      );
      const meals = await Promise.all(mealRequests);
      setSelectedMeals(meals.filter(Boolean));
      setSelectedMealId(null);
      setSelectedMealFoods([]);
    } catch (error) {
      setSelectedMeals([]);
      setSelectedMealId(null);
      setSelectedMealFoods([]);
    }
  };

  const handleMealClick = async (meal) => {
    setSelectedMealId(meal.Id);
    try {
      const response = await baseApi.getAll("/food", {
        mealId: meal.Id,
        pageNumber: 1,
        pageSize: 100,
      });
      setSelectedMealFoods(response.data.Items || []);
    } catch (error) {
      setSelectedMealFoods([]);
    }
  };

  const handleAddFoodClick = (mealId) => {
    setFoodModalMealId(mealId);
    setFoodModalOpen(true);
  };

  const handleFoodAdded = async () => {
    setFoodModalOpen(false);
    if (selectedMealId) {
      await handleMealClick({ Id: selectedMealId });
    }
  };

  const handleMealPlanAdded = () => {
    mealPlanStore.fetchAll();
  };

  const addMealPlanButton = (
    <button
      style={{
        background: "#4f8cff",
        color: "#fff",
        border: "none",
        borderRadius: "6px",
        padding: "8px 20px",
        fontSize: "1em",
        fontWeight: 600,
        cursor: "pointer",
        boxShadow: "0 2px 8px #4f8cff22",
        transition: "background 0.18s, box-shadow 0.18s",
      }}
      onMouseOver={e => e.currentTarget.style.background = "#356ad6"}
      onMouseOut={e => e.currentTarget.style.background = "#4f8cff"}
      onClick={() => setAddModalOpen(true)}
    >
      ➕ ADD MEAL PLAN
    </button>
  );

  const addMealButton = (
    <button
      style={{
        background: "#4f8cff",
        color: "#fff",
        border: "none",
        borderRadius: "6px",
        padding: "8px 20px",
        fontSize: "1em",
        fontWeight: 600,
        cursor: "pointer",
        boxShadow: "0 2px 8px #4f8cff22",
        transition: "background 0.18s, box-shadow 0.18s",
        marginLeft: 16
      }}
      onClick={() => setAddMealModalOpen(true)}
    >
      ➕ ADD MEAL
    </button>
  );

  return (
    <div style={{ padding: 20 }}>
      <h2>📋 MY MEAL PLANS</h2>
      <div className="mealplan-table-wrapper">
        <GenericTable
          store={mealPlanStore}
          onRowClick={handleMealPlanClick}
          headerButton={addMealPlanButton}
          onDeleteRow={id => openDeleteModal("mealplan", id)}
        />
      </div>

      {currentMealPlan && (
        <div>
          <div style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            marginBottom: 12,
            gap: 16
          }}>
            <h3 style={{ margin: 0 }}>
              🍽️ MEALS IN {currentMealPlan.Name ? `"${currentMealPlan.Name}"` : "SELECTED PLAN"}
            </h3>
            {addMealButton}
          </div>
          <div className="meals-container">
            {selectedMeals.length === 0 ? (
              <div style={{ textAlign: "center", marginTop: 32 }}>
                <p style={{ fontSize: "1.1em", color: "#888", marginBottom: 18 }}>
                  No meals added yet.
                </p>
              </div>
            ) : (
              selectedMeals.map((meal) => (
                <div key={meal.Id} className="meal-card">
                  <div
                    className={`meal-title${selectedMealId === meal.Id ? " selected" : ""}`}
                    style={{
                      cursor: "pointer",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "space-between",
                      width: "100%"
                    }}
                    onClick={() => handleMealClick(meal)}
                  >
                    <div>
                      <strong>{meal.Name}</strong>
                      {meal.Description ? (
                        <span className="meal-description"> - {meal.Description}</span>
                      ) : null}
                    </div>
                    <button
                      onClick={e => { e.stopPropagation(); openDeleteModal("meal", meal.Id); }}
                      style={deleteButtonStyle}
                      title="Delete meal"
                    >×</button>
                  </div>
                  {selectedMealId === meal.Id && (
                    <>
                      <button
                        style={{
                          marginTop: 8,
                          marginBottom: 8,
                          background: "#4f8cff",
                          color: "#fff",
                          border: "none",
                          borderRadius: "6px",
                          padding: "8px 20px",
                          fontSize: "1em",
                          fontWeight: 600,
                          cursor: "pointer",
                          boxShadow: "0 2px 8px #4f8cff22",
                          transition: "background 0.18s, box-shadow 0.18s",
                        }}
                        onClick={() => handleAddFoodClick(meal.Id)}
                      >
                        ➕ ADD FOOD
                      </button>
                      <div className="foods-container">
                        {selectedMealFoods.length > 0 ? (
                          selectedMealFoods.map((food) => (
                            <div
                              className="food-item"
                              key={food.Id}
                              style={{
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "space-between",
                                width: "100%"
                              }}
                            >
                              <div>
                                <span className="food-name">{food.Name}</span>
                                {food.Weight && (
                                  <span className="food-weight">({food.Weight}g)</span>
                                )}
                                {food.Description && (
                                  <span className="food-desc">- {food.Description}</span>
                                )}
                              </div>
                              <button
                                onClick={e => { e.stopPropagation(); openDeleteModal("food", food.Id); }}
                                style={deleteButtonStyle}
                                title="Delete food"
                              >×</button>
                            </div>
                          ))
                        ) : (
                          <div className="food-item empty">THERE IS NO FOOD ADDED IN THIS MEAL!</div>
                        )}
                      </div>
                    </>
                  )}
                </div>
              ))
            )}
          </div>
        </div>
      )}

      {foodModalOpen && (
        <FoodFormModal
          mealId={foodModalMealId}
          onClose={() => setFoodModalOpen(false)}
          onFoodAdded={handleFoodAdded}
        />
      )}

      {addModalOpen && (
        <AddMealPlanModal
          open={addModalOpen}
          onClose={() => setAddModalOpen(false)}
          onAdded={handleMealPlanAdded}
        />
      )}

      {addMealModalOpen && (
        <AddMealModal
          open={addMealModalOpen}
          onClose={() => setAddMealModalOpen(false)}
          mealPlanId={currentMealPlan ? currentMealPlan.Id : null}
          onAdded={async () => {
            setAddMealModalOpen(false);
            if (currentMealPlan) {
              await handleMealPlanClick(currentMealPlan);
            }
          }}
        />
      )}

      <ConfirmDeleteModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        onConfirm={handleDeleteConfirmed}
        text={
          deleteTarget.type === "mealplan"
            ? "This will remove the meal plan and all its meals and foods."
            : deleteTarget.type === "meal"
              ? "This will remove the meal and all its foods."
              : "This will remove the food."
        }
      />
    </div>
  );
});

export default MealPlanPage;
