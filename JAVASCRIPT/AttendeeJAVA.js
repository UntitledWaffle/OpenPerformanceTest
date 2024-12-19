// Base URL for the API
const APIBaseURL = 'http://localhost:5000/api/attendees';

// Fetch all attendees and display in the table
async function fetchAttendees() {
    try {
        const response = await fetch(APIBaseURL);
        const attendees = await response.json();
        const tableBody = document.getElementById('attendees-table').querySelector('tbody');
        tableBody.innerHTML = '';

        attendees.forEach(attendee => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${attendee.name}</td>
                <td>${attendee.email}</td>
                <td>${attendee.event}</td>
                <td>
                    <button onclick="deleteAttendee('${attendee.id}')">Delete</button>
                    <button onclick="editAttendee('${attendee.id}')">Edit</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Error fetching attendees:', error);
    }
}

// Add a new attendee
async function addAttendee(event) {
    event.preventDefault();
    const name = document.getElementById('attendee-name').value;
    const email = document.getElementById('attendee-email').value;
    const eventName = document.getElementById('attendee-event').value;

    try {
        await fetch(APIBaseURL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, email, event: eventName })
        });
        fetchAttendees();
        event.target.reset();
    } catch (error) {
        console.error('Error adding attendee:', error);
    }
}

// Delete an attendee
async function deleteAttendee(id) {
    try {
        await fetch(`${APIBaseURL}/${id}`, { method: 'DELETE' });
        fetchAttendees();
    } catch (error) {
        console.error('Error deleting attendee:', error);
    }
}

// Event listener for the add attendee form
document.getElementById('add-attendee-form').addEventListener('submit', addAttendee);

// Fetch attendees on page load
window.onload = fetchAttendees;
