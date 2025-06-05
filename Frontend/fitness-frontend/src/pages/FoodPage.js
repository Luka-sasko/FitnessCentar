import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { foodStore } from "../stores/FoodStore";
import GenericTable from "../components/Common/GenericTable";

const FoodPage = observer(() => {
  useEffect(() => {
    foodStore.fetchAll();
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h2>🍽️ Food Page</h2>
      <GenericTable store={foodStore} />
    </div>
  );
});

export default FoodPage;
