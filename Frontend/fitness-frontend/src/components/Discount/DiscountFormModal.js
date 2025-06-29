import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { discountStore } from "../../stores/DiscountStore";

const DiscountFormModal = observer(({ isOpen, onClose, initialData }) => {
    const isEditMode = !!initialData;



    const [form, setForm] = useState({
        name: "",
        amount: 0,
        startDate: "",
        endDate: "",
        isActive: true,
    });

    useEffect(() => {
        if (isEditMode) {
            setForm({
                name: initialData.Name || "",
                amount: initialData.Amount || 0,
                startDate: initialData.StartDate?.replace(" ", "T") || "",
                endDate: initialData.EndDate?.replace(" ", "T") || "",
                isActive: initialData.IsActive ?? true,
            });
        } else {
            setForm({
                name: "",
                amount: 0,
                startDate: "",
                endDate: "",
                isActive: true,
            });
        }
    }, [initialData, isEditMode]);



    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const data = {
            Name: form.name,
            Amount: parseFloat(form.amount),
            StartDate: form.startDate,
            EndDate: form.endDate,
        };

        if (isEditMode) {
            data.IsActive = form.isActive;
            console.log(initialData.id);
            await discountStore.updateDiscount(initialData.Id, data);
        } else {
            await discountStore.createDiscount(data);
        }

        onClose();
    };

    if (!isOpen) return null;

    return (
        <div className="modal">
            <form onSubmit={handleSubmit} className="modal-content">
                <h2>{isEditMode ? "Edit Discount" : "Create Discount"}</h2>

                <label>Name</label>
                <input
                    type="text"
                    name="name"
                    value={form.name}
                    onChange={handleChange}
                    required
                />

                <label>Amount (%)</label>
                <input
                    type="number"
                    name="amount"
                    value={form.amount}
                    onChange={handleChange}
                    required
                />

                <label>Start Date</label>
                <input
                    type="datetime-local"
                    name="startDate"
                    value={form.startDate}
                    onChange={handleChange}
                    required
                />

                <label>End Date</label>
                <input
                    type="datetime-local"
                    name="endDate"
                    value={form.endDate}
                    onChange={handleChange}
                    required
                />

                {isEditMode && (
                    <label>
                        <input
                            type="checkbox"
                            name="isActive"
                            checked={form.isActive}
                            onChange={handleChange}
                        />
                        Active
                    </label>
                )}

                <div className="modal-actions">
                    <button type="submit">Save</button>
                    <button type="button" onClick={onClose}>
                        Cancel
                    </button>
                </div>
            </form>
        </div>
    );
});

export default DiscountFormModal;
