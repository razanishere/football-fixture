const BASE_URL = "http://localhost:5201/api";

export const getFixtures = async () => {
  const res = await fetch(`${BASE_URL}/simulation/get-fixtures`);

  if (!res.ok) {
    throw new Error("Failed to fetch fixtures");
  }

  return await res.json();
};