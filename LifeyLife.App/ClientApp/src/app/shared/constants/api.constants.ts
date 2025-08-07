export const API_BASE_URL = 'https://localhost:5555/api';

export const API_ENDPOINTS = {
  LOGIN: `${API_BASE_URL}/accounts/login`,
  REGISTRATION: `${API_BASE_URL}/accounts/register`,
  RANDOM_DARE: `${API_BASE_URL}/random-dare`,
  RANDOM_DARE_HISTORY: `${API_BASE_URL}/random-dare-history`,
  COMPLETE_DARE: (uuid: string) => `${API_BASE_URL}/random-dare/Complete`
}; 