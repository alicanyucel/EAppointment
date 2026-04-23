import http from 'k6/http';
import { check } from 'k6';

// Stress test - gradually increasing load until system breaks
export const options = {
  stages: [
    { duration: '2m', target: 100 },   // Ramp-up to 100 users
    { duration: '5m', target: 100 },   // Stay at 100 for 5 minutes
    { duration: '2m', target: 200 },   // Ramp-up to 200 users
    { duration: '5m', target: 200 },   // Stay at 200 for 5 minutes
    { duration: '2m', target: 300 },   // Ramp-up to 300 users
    { duration: '5m', target: 300 },   // Stay at 300 for 5 minutes
    { duration: '2m', target: 400 },   // Ramp-up to 400 users
    { duration: '5m', target: 400 },   // Stay at 400 for 5 minutes
    { duration: '10m', target: 0 },    // Recovery
  ],
  thresholds: {
    'http_req_failed': ['rate<0.05'],  // Error rate must be less than 5%
  },
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000';

export default function () {
  const response = http.get(`${BASE_URL}/api/doctors/getall`);
  
  check(response, {
    'status is 200': (r) => r.status === 200,
  });
}
