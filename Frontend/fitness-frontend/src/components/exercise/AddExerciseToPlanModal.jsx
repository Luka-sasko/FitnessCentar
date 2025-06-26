import React, { useEffect, useState } from "react";
import baseApi from "../../api/BaseApi";

const AddExerciseToPlanModal = ({ open, onClose, workoutPlanId, onAdded }) => {
    const [mode, setMode] = useState("list");
    const [exercises, setExercises] = useState([]);
    const [selectedExerciseId, setSelectedExerciseId] = useState("");
    const [newExercise, setNewExercise] = useState({
        Name: "",
        Desc: "",
        Reps: "",
        Sets: "",
        RestPeriod: ""
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    useEffect(() => {
        if (!open) return;
        setMode("list");
        setSelectedExerciseId("");
        setNewExercise({
            Name: "",
            Desc: "",
            Reps: "",
            Sets: "",
            RestPeriod: ""
        });
        setError("");
        setLoading(false);
        const fetchExercises = async () => {
            try {
                const res = await baseApi.getAll("/exercise", { pageSize: 100 });
                setExercises(res.data.Items || []);
            } catch {
                setExercises([]);
            }
        };
        fetchExercises();
    }, [open]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        try {
            let exerciseId;
            if (mode === "list") {
                if (!selectedExerciseId) {
                    setError("Please select an exercise.");
                    setLoading(false);
                    return;
                }
                exerciseId = selectedExerciseId;
            } else {
                if (!newExercise.Name || !newExercise.Reps || !newExercise.Sets) {
                    setError("Please fill all required fields.");
                    setLoading(false);
                    return;
                }
                const res = await baseApi.post("/exercise", {
                    Name: newExercise.Name,
                    Desc: newExercise.Desc,
                    Reps: Number(newExercise.Reps),
                    Sets: Number(newExercise.Sets),
                    RestPeriod: Number(newExercise.RestPeriod)
                });
                exerciseId = res.data;
            }


            await baseApi.post("/workoutplanexercise", {
                WorkoutPlanId: workoutPlanId,
                ExerciseId: exerciseId,
                ExerciseNumber: 1
            });

            setLoading(false);
            setSelectedExerciseId("");
            setNewExercise({
                Name: "",
                Desc: "",
                Reps: "",
                Sets: "",
                RestPeriod: ""
            });
            onAdded();
            onClose();
        } catch (err) {
            setError("Error adding exercise.");
            setLoading(false);
        }
    };

    if (!open) return null;

    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <button className="modal-close" onClick={onClose}>&times;</button>
                <h3 className="modal-title">ADD EXERCISE TO PLAN</h3>

                <div style={{ display: "flex", gap: 18, marginBottom: 18 }}>
                    <button
                        type="button"
                        style={{
                            background: mode === "list" ? "#4f8cff" : "#e0e7ef",
                            color: mode === "list" ? "#fff" : "#223",
                            border: "none",
                            borderRadius: "6px",
                            padding: "8px 18px",
                            fontWeight: 600,
                            cursor: "pointer"
                        }}
                        onClick={() => setMode("list")}
                    >
                        ADD FROM LIST
                    </button>
                    <button
                        type="button"
                        style={{
                            background: mode === "new" ? "#4f8cff" : "#e0e7ef",
                            color: mode === "new" ? "#fff" : "#223",
                            border: "none",
                            borderRadius: "6px",
                            padding: "8px 18px",
                            fontWeight: 600,
                            cursor: "pointer"
                        }}
                        onClick={() => setMode("new")}
                    >
                        ADD NEW
                    </button>
                </div>

                <form onSubmit={handleSubmit} className="modal-form">
                    {mode === "list" ? (
                        <label className="exercise-select-label">
                            SELECT EXERCISE:
                            <select
                                className="exercise-select"
                                value={selectedExerciseId}
                                onChange={e => setSelectedExerciseId(e.target.value)}
                                required
                            >
                                <option value="">-- SELECT --</option>
                                {exercises.map(ex => (
                                    <option key={ex.Id} value={ex.Id}>{ex.Name}</option>
                                ))}
                            </select>
                        </label>
                    ) : (
                        <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
                            <label>
                                NAME: <span style={{ color: "#e03c3c" }}>*</span>
                                <input
                                    type="text"
                                    value={newExercise.Name}
                                    onChange={e => setNewExercise({ ...newExercise, Name: e.target.value })}
                                    required
                                />
                            </label>
                            <label>
                                DESCRIPTION:
                                <input
                                    type="text"
                                    value={newExercise.Desc}
                                    onChange={e => setNewExercise({ ...newExercise, Desc: e.target.value })}
                                />
                            </label>
                            <label>
                                REPS: <span style={{ color: "#e03c3c" }}>*</span>
                                <input
                                    type="number"
                                    min={1}
                                    value={newExercise.Reps}
                                    onChange={e => setNewExercise({ ...newExercise, Reps: e.target.value })}
                                    required
                                />
                            </label>
                            <label>
                                SETS: <span style={{ color: "#e03c3c" }}>*</span>
                                <input
                                    type="number"
                                    min={1}
                                    value={newExercise.Sets}
                                    onChange={e => setNewExercise({ ...newExercise, Sets: e.target.value })}
                                    required
                                />
                            </label>
                            <label>
                                REST PERIOD (sec):
                                <input
                                    type="number"
                                    min={0}
                                    value={newExercise.RestPeriod}
                                    onChange={e => setNewExercise({ ...newExercise, RestPeriod: e.target.value })}
                                />
                            </label>
                        </div>
                    )}

                    {error && <div className="modal-error">{error}</div>}
                    <div className="modal-actions">
                        <button type="submit" disabled={loading || (mode === "list" && !selectedExerciseId) || (mode === "new" && (!newExercise.Name || !newExercise.Reps || !newExercise.Sets))}>
                            {loading ? "ADDING..." : "ADD EXERCIS"}
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

export default AddExerciseToPlanModal;
