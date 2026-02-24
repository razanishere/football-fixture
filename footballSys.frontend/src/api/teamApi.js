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

