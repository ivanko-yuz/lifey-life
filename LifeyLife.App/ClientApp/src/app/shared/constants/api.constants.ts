import { environment } from '../../../environments/environment';

const BASE = environment.apiBaseUrl;

export const API_ENDPOINTS = {
  LOGIN: `${BASE}/accounts/login`,
  REGISTRATION: `${BASE}/accounts/register`,
  RANDOM_DARE: `${BASE}/random-dare`,
  RANDOM_DARE_HISTORY: `${BASE}/random-dare-history`,
  COMPLETE_DARE: `${BASE}/random-dare/Complete`,
  GET_PROFILE: `${BASE}/accounts/profile`,
  UPDATE_LANGUAGE: `${BASE}/accounts/language`,
  CHARACTER: `${BASE}/character`,
  LEADERBOARD: `${BASE}/leaderboard`,
  TODO: `${BASE}/todo`,
  TODO_HISTORY: `${BASE}/todo/history`,
  TODO_FINISH_DAY: `${BASE}/todo/finish-day`
};