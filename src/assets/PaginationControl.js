import React from "react";
import MuiPagination from "@mui/material/Pagination";

const PaginationControl = ({ count, page, onChange }) => {
  return (
    <div className="d-flex justify-content-end mt-3">
      <MuiPagination
        count={count}
        page={page}
        onChange={onChange}
        sx={{
          "& .MuiPaginationItem-root": {
            color: "#4282be",
          },
          "& .MuiPaginationItem-root:hover": {
            backgroundColor: "#d0e1ff",
          },
          "& .MuiPaginationItem-root.Mui-selected": {
            backgroundColor: "#4282be",
            color: "#fff",
            "&:hover": {
              backgroundColor: "#2a5c91",
            },
          },
        }}
      />
    </div>
  );
};

export default PaginationControl;
