import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { foodStore } from "../stores/FoodStore";
import GenericTable from "../components/Common/GenericTable";
import FoodForm from "../components/food/FoodForm";

const FoodPage = observer(() => {
  useEffect(() => {
    foodStore.fetchAll();
  }, []);

  const handleSubmit = async (data) => {
    await foodStore.createFood(data);
    foodStore.setDialogOpen(false);
  };

  return (
    <div style={{ padding: 20 }}>
      <h2>🍽️ Food Page</h2>
      <button style={{ marginBottom: '16px' }} onClick={() => foodStore.setDialogOpen(true)}>➕ ADD FOOD</button>

      {foodStore.dialogOpen && (
        <FoodForm
          onSubmit={handleSubmit}
          onClose={() => foodStore.setDialogOpen(false)}
        />
      )}
      <GenericTable store={foodStore} />
    </div>
  );
});

export default FoodPage;
