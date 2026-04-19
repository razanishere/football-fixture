

import { useState } from "react";
import AddTeamForm from "./addTeamForm";
import TeamsModal from "./teams/teamModal.jsx";

const AddTeamButton = () => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <button onClick={() => setIsOpen(true)}>
        Add Team
      </button>

      {isOpen && (
        <TeamsModal title="Add New Team" onClose={() => setIsOpen(false)}>
          <AddTeamForm onClose={() => setIsOpen(false)} />
        </TeamsModal>
      )}
    </>
  );
};

export default AddTeamButton;