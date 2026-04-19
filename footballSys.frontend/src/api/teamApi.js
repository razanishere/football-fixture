import axios from "axios";

const API_BASE_URL = "http://localhost:5201";

// GET
export const getTeams = async () => {
  const response = await axios.get(`${API_BASE_URL}/intro`);
  return response.data;
};


// POST multipart
export const createTeam = async (newTeam) => {
  const formData = new FormData();

  formData.append("teamName", newTeam.teamName);
  formData.append("yearEstablished", newTeam.yearEstablished);
  formData.append("teamColor1", newTeam.teamColor1);
  formData.append("teamColor2", newTeam.teamColor2);
  formData.append("logo", newTeam.logo); // File object

  const response = await axios.post(
    `${API_BASE_URL}/api/teams`,
    formData,
    {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
  );

  return response.data;
};

//DELETE 
export const deleteTeam = async (id) => {
  await axios.delete(`${API_BASE_URL}/intro/${id}`);
};

//UPDATE
//! check this for deletion
// export const updateTeamLogo = async (id, formData) => {
//   const res = await fetch(`http://localhost:5201/api/teams/${id}`, {
//     method: "POST",
//     body: formData,
//   });

//   if (!res.ok) throw new Error("Logo update failed");
//   return res.json();
// };


// UPDATE team info (PUT request)
export const updateTeam = async (id, updatedTeam, file) => {
  const formData = new FormData();

  formData.append("teamName", updatedTeam.teamName);
  formData.append("yearEstablished", updatedTeam.yearEstablished);
  formData.append("teamColor1", updatedTeam.teamColor1);
  formData.append("teamColor2", updatedTeam.teamColor2);

  if (file) {
    formData.append("logo", file); // MUST be named "logo"
  }

  const response = await axios.put(
    `http://localhost:5201/api/teams/${id}`,
    formData,
    { headers: { "Content-Type": "multipart/form-data" } }
  );

  return response.data;
};