// Base URL for the API
const APIBaseURL = 'http://localhost:5000/api/events';

// Fetch all events and display in the table
async function fetchEvents() {
    try {
        const response = await fetch(APIBaseURL);
        const events = await response.json();
        const tableBody = document.getElementById('events-table').querySelector('tbody');
        tableBody.innerHTML = '';

        events.forEach(event => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${event.name}</td>
                <td>${event.date}</td>
                <td>${event.location}</td>
                <td>
                    <button onclick="deleteEvent('${event.id}')">Delete</button>
                    <button onclick="editEvent('${event.id}')">Edit</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Error fetching events:', error);
    }
}

// Add a new event
async function addEvent(event) {
    event.preventDefault();
    const name = document.getElementById('event-name').value;
    const date = document.getElementById('event-date').value;
    const location = document.getElementById('event-location').value;

    try {
        await fetch(APIBaseURL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, date, location })
        });
        fetchEvents();
        event.target.reset();
    } catch (error) {
        console.error('Error adding event:', error);
    }
}

// Delete an event
async function deleteEvent(id) {
    try {
        await fetch(`${APIBaseURL}/${id}`, { method: 'DELETE' });
        fetchEvents();
    } catch (error) {
        console.error('Error deleting event:', error);
    }
}

// Edit an event (placeholder for implementation)
function editEvent(id) {
    alert(`Edit functionality not yet implemented for event ID: ${id}`);
}

// Event listener for the add event form
document.getElementById('add-event-form').addEventListener('submit', addEvent);

// Fetch events on page load
window.onload = fetchEvents;
