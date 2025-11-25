import React, { useEffect, useState } from "react";
import { IconSearch } from "@tabler/icons-react";

export const Search = ({ tableIds }) => {
  const [query, setQuery] = useState("");

  useEffect(() => {
    tableIds.forEach((tableId) => {
      const table = document.getElementById(tableId);
      if (!table) return;

      const rows = table.querySelectorAll("tbody tr");

      rows.forEach((row) => {
        const cells = row.getElementsByTagName("td");
        let match = false;

        for (let i = 0; i < cells.length; i++) {
          if (
            cells[i].textContent.toLowerCase().includes(query.toLowerCase())
          ) {
            match = true;
            break;
          }
        }

        row.style.display = match ? "" : "none";
      });
    });
  }, [query, tableIds]);

  return (
    <form action="#" method="get" autoComplete="off" noValidate>
      <div className="input-icon">
        <span className="input-icon-addon">
          <IconSearch size={24} stroke={2} />
        </span>
        <input
          type="text"
          className="form-control search-input"
          placeholder="Buscar…"
          aria-label="Buscar"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
        />
      </div>
    </form>
  );
};
