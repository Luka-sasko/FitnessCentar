import React from "react";
import { observer } from "mobx-react-lite";

const GenericTable = observer(({ store }) => {
    const data = store.items ?? [];

    if (!data || data.length === 0) return <p>🔍 Nema podataka.</p>;
    const hiddenColumns = ["id", "datecreated", "updatedby", "createdby", "updatedat", "dateupdated", "isactive"];
    const columns = Object.keys(data[0] || {}).filter(
        (key) => !hiddenColumns.includes(key.toLowerCase())
    );
    /* const hiddenColumns = ["id", "datecreated", "updatedby", "createdby", "updatedat", "dateupdated", "isactive"];

    const columns = Object.keys(data[0]).filter(
        (key) => !hiddenColumns.includes(key.toLowerCase())
    ); */

    const handleSort = (col) => {
        console.log(col, store.sortOrder.toLowerCase() === "asc" ? "desc" : "asc");
        store.setSort(col, store.sortOrder.toLowerCase() === "asc" ? "desc" : "asc");
    };

    const handlePageChange = (newPage) => {
        if (newPage >= 1 && newPage <= store.totalPages && newPage !== store.currentPage) {
            store.setCurrentPage(newPage);
        }
    };

    return (
        <div>
            <table border="1" cellPadding="8" cellSpacing="0" style={{ width: "100%" }}>
                <thead>
                    <tr>
                        {columns.map((col) => (
                            <th
                                key={col}
                                onClick={() => handleSort(col)}
                                style={{ cursor: "pointer" }}
                            >
                                {col}{" "}
                                {store.sortBy === col
                                    ? store.sortOrder.toLowerCase() === "asc"
                                        ? "↑"
                                        : "↓"
                                    : ""}
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, idx) => (
                        <tr key={idx}>
                            {columns.map((col) => (
                                <td key={col}>{String(item[col])}</td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </table>

            <div style={{ marginTop: 10 }}>
                <button onClick={() => handlePageChange(store.currentPage - 1)} disabled={store.currentPage <= 1}>
                    ⬅️
                </button>
                <span style={{ margin: "0 10px" }}>
                    Stranica {store.currentPage} / {store.totalPages}
                </span>
                <button onClick={() => handlePageChange(store.currentPage + 1)} disabled={store.currentPage >= store.totalPages}>
                    ➡️
                </button>
            </div>
        </div>
    );
});

export default GenericTable;
