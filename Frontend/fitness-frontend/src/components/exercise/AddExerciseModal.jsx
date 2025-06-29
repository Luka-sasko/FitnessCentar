import React, { useEffect, useState } from "react";

const AddExerciseModal = ({ open, onClose, initialData = null, onSubmit }) => {
  const isEditMode = !!initialData;

  const [form, setForm] = useState({
    Name: "",
    Desc: "",
    Reps: "",
    Sets: "",
    RestPeriod: "",
  });

  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (isEditMode) {
      setForm({
        Name: initialData.Name ?? "",
        Desc: initialData.Desc ?? "",
        Reps: initialData.Reps ?? "",
        Sets: initialData.Sets ?? "",
        RestPeriod: initialData.RestPeriod ?? "",
      });
    } else {
      setForm({
        Name: "",
        Desc: "",
        Reps: "",
        Sets: "",
        RestPeriod: "",
      });
    }
  }, [initialData, isEditMode]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    const payload = {
      Name: form.Name,
      Desc: form.Desc,
      Reps: parseInt(form.Reps),
      Sets: parseInt(form.Sets),
      RestPeriod: parseInt(form.RestPeriod),
    };

    await onSubmit(payload);
    setLoading(false);
  };

  if (!open) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-glass">
        <button className="modal-close" onClick={onClose}>&times;</button>
        <h3 className="modal-title">{isEditMode ? "EDIT EXERCISE" : "ADD EXERCISE"}</h3>
        <form
          onSubmit={handleSubmit}
          style={{ display: "flex", flexDirection: "column", gap: 10, color: "black" }}
          className="modal-form"
        >
          <label>
            NAME:
            <input
              name="Name"
              type="text"
              value={form.Name}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            DESCRIPTION:
            <input
              name="Desc"
              type="text"
              value={form.Desc}
              onChange={handleChange}
            />
          </label>
          <label>
            REPS:
            <input
              name="Reps"
              type="number"
              value={form.Reps}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            SETS:
            <input
              name="Sets"
              type="number"
              value={form.Sets}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            REST PERIOD (sec):
            <input
              name="RestPeriod"
              type="number"
              value={form.RestPeriod}
              onChange={handleChange}
            />
          </label>
          <div className="modal-actions">
            <button type="submit" disabled={loading}>
              {loading ? (isEditMode ? "UPDATING..." : "ADDING...") : isEditMode ? "UPDATE" : "ADD"}
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

export default AddExerciseModal;
