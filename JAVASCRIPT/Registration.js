// Base URL for the API
const APIBaseURL = 'http://localhost:5000/api/registrations';

// Fetch all registrations and display them in the table
async function fetchRegistrations() {
    try {
        const response = await fetch(APIBaseURL);
        const registrations = await response.json();
        const tableBody = document.getElementById('registrations-table').querySelector('tbody');
        tableBody.innerHTML = '';

        registrations.forEach(registration => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${registration.attendee}</td>
                <td>${registration.event}</td>
                <td>${registration.status}</td>
                <td>
                    <button onclick="deleteRegistration('${registration.id}')">Delete</button>
                    <button onclick="editRegistration('${registration.id}')">Edit</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Error fetching registrations:', error);
    }
}

// Add a new registration
async function addRegistration(event) {
    event.preventDefault();
    const attendee = document.getElementById('registration-attendee').value;
    const eventName = document.getElementById('registration-event').value;
    const status = document.getElementById('registration-status').value;

    try {
        await fetch(APIBaseURL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ attendee, event: eventName, status })
        });
        fetchRegistrations();
        event.target.reset();
    } catch (error) {
        console.error('Error adding registration:', error);
    }
}

// Delete a registration
async function deleteRegistration(id) {
    try {
        await fetch(`${APIBaseURL}/${id}`, { method: 'DELETE' });
        fetchRegistrations();
    } catch (error) {
        console.error('Error deleting registration:', error);
    }
}

// Update a registration
async function updateRegistration(id, updatedData) {
    try {
        await fetch(`${APIBaseURL}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedData)
        });
        fetchRegistrations(); // Refresh registration list
    } catch (error) {
        console.error('Error updating registration:', error);
    }
}

// Edit a registration
function editRegistration(id) {
    const attendee = prompt('Enter new attendee name:');
    const eventName = prompt('Enter new event name:');
    const status = prompt('Enter new status (Scheduled, Completed, Cancelled):');

    if (attendee && eventName && status) {
        updateRegistration(id, { attendee, event: eventName, status });
    } else {
        alert('All fields are required for updating!');
    }
}

// Event listener for the add registration form
document.getElementById('add-registration-form').addEventListener('submit', addRegistration);

// Fetch registrations on page load
window.onload = fetchRegistrations;