import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { discountStore } from "../stores/DiscountStore";
import GenericTable from "../components/Common/GenericTable";
import { userStore } from "../stores/UserStore.js";
import DiscountFormModal from "../components/Discount/DiscountFormModal.js";
import '../styles/discount.css';

const DiscountPage = observer(() => {
  const [modalOpen, setModalOpen] = useState(false);
  const [editData, setEditData] = useState(null);

  useEffect(() => {
    discountStore.fetchAll();
  }, []);

    if (!userStore.isAdmin) {
    return (<div style={{marginTop: "10%"}}><h1>Access denied</h1></div>);
  }

  const handleSubmit = async (data) => {
    if (editData) {
      await discountStore.updateDiscount(editData.Id || editData.id, data);
    } else {
      await discountStore.createDiscount(data);
    }
    setModalOpen(false);
    setEditData(null);
  };

  const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this discount?")) {
      await discountStore.deleteDiscount(id);
    }
  };

  const handleEdit = (item) => {
    setEditData(item);
    setModalOpen(true);
  };

  const handleAdd = () => {
    setEditData(null);
    setModalOpen(true);
  };

  const addButton = (
    <button
      style={{
        background: "#000", color: "#fff", padding: "8px 16px",
        borderRadius: "6px", border: "1px solid #000", fontWeight: "bold"
      }}
      onClick={handleAdd}
    >
      ➕ ADD DISCOUNT
    </button>
  );

  return (
    <div style={{ padding: 20 }}>
      <h2>🎟️ Discount Management</h2>
      <GenericTable
        store={discountStore}
        onRowClick={handleEdit}
        onDeleteRow={handleDelete}
        headerButton={addButton}
      />
      <DiscountFormModal
        isOpen={modalOpen}
        onClose={() => {
          setModalOpen(false);
          setEditData(null);
        }}
        onSubmit={handleSubmit}
        initialData={editData}
      />
    </div>
  );
});

export default DiscountPage;
