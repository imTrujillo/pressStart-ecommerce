import { IconMoodSadFilled } from "@tabler/icons-react";
import React from "react";

export const EmptyState = ({ text }) => {
  return (
    <div className="d-flex flex-column align-items-center justify-content-center">
      <IconMoodSadFilled size={48} />
      <span className="fw-semibold">{text}</span>
    </div>
  );
};
