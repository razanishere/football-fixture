// import { useState } from "react";
// import AddTeamForm from "../components/addTeamForm";

// const AddTeamButton = () => {
//   const [showForm, setShowForm] = useState(false);

//   const handleClick = () => {
//     setShowForm(true);
//   };

//   const handleClose = () => {
//     setShowForm(false);
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>
//         Add Team
//       </button>

//       {showForm && (
//         <AddTeamForm onClose={handleClose} />
//       )}
//     </div>
//   );
// };

// export default AddTeamButton;

// //TODO: make the frotnend better then start the update button

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