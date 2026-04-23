import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');

// Test configuration
export const options = {
  stages: [
    { duration: '30s', target: 10 },  // Ramp up to 10 users
    { duration: '1m', target: 50 },   // Ramp up to 50 users
    { duration: '2m', target: 100 },  // Ramp up to 100 users
    { duration: '1m', target: 200 },  // Ramp up to 200 users
    { duration: '2m', target: 200 },  // Stay at 200 users
    { duration: '1m', target: 0 },    // Ramp down to 0 users
  ],
  thresholds: {
    'http_req_duration': ['p(95)<500', 'p(99)<1000'], // 95% of requests must complete below 500ms
    'http_req_failed': ['rate<0.01'],  // Error rate must be less than 1%
    'errors': ['rate<0.1'],            // Custom error rate
  },
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000';

export default function () {
  // Test 1: Get all doctors
  let doctorsResponse = http.get(`${BASE_URL}/api/doctors/getall`);
  
  check(doctorsResponse, {
    'get doctors status is 200': (r) => r.status === 200,
    'get doctors response time < 500ms': (r) => r.timings.duration < 500,
  }) || errorRate.add(1);

  sleep(1);

  // Test 2: Get all patients
  let patientsResponse = http.get(`${BASE_URL}/api/patients/getall`);
  
  check(patientsResponse, {
    'get patients status is 200': (r) => r.status === 200,
    'get patients response time < 500ms': (r) => r.timings.duration < 500,
  }) || errorRate.add(1);

  sleep(1);

  // Test 3: Login endpoint
  const loginPayload = JSON.stringify({
    email: 'test@example.com',
    password: 'Test123!'
  });

  const loginParams = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  let loginResponse = http.post(`${BASE_URL}/api/auth/login`, loginPayload, loginParams);
  
  check(loginResponse, {
    'login response time < 1000ms': (r) => r.timings.duration < 1000,
  }) || errorRate.add(1);

  sleep(2);
}

// Setup function - runs once before the test
export function setup() {
  console.log('Starting load test...');
  console.log(`Base URL: ${BASE_URL}`);
  return { startTime: new Date() };
}

// Teardown function - runs once after the test
export function teardown(data) {
  const endTime = new Date();
  const duration = (endTime - data.startTime) / 1000;
  console.log(`Load test completed in ${duration} seconds`);
}
