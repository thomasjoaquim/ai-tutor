// AI Biography Assistant - 3 Step Process
let biographyData = {};
let generatedText = '';

function toggleAIAssistant() {
    const container = document.getElementById('aiAssistantContainer');
    container.style.display = container.style.display === 'none' ? 'block' : 'none';
}

function goToStep2() {
    const q1 = document.getElementById('q1').value;
    const q2 = document.getElementById('q2').value;
    const q3 = document.getElementById('q3').value;
    const q4 = document.getElementById('q4').value;
    const q5 = document.getElementById('q5').value;
    
    if (!q1 || !q2 || !q3 || !q4 || !q5) {
        alert('Please answer all 5 questions before continuing.');
        return;
    }
    
    biographyData = { q1, q2, q3, q4, q5 };
    document.getElementById('questionsPhase').style.display = 'none';
    document.getElementById('stylePhase').style.display = 'block';
}

function backToStep1() {
    document.getElementById('stylePhase').style.display = 'none';
    document.getElementById('questionsPhase').style.display = 'block';
}

function backToStep2() {
    document.getElementById('previewPhase').style.display = 'none';
    document.getElementById('stylePhase').style.display = 'block';
}

async function generateBiography() {
    const tone = document.getElementById('tone').value;
    const length = document.getElementById('length').value;
    
    document.getElementById('stylePhase').style.display = 'none';
    document.getElementById('previewPhase').style.display = 'block';
    
    const lengthWords = length === 'short' ? '100-150' : length === 'medium' ? '200-300' : '400-500';
    const toneDesc = {
        'formal': 'formal and respectful',
        'warm': 'warm and personal',
        'celebratory': 'celebratory and uplifting',
        'poetic': 'poetic and reflective'
    }[tone];
    
    const prompt = `Write a ${toneDesc} biography (${lengthWords} words) for ${biographyData.q1}. ` +
                  `Their passions were: ${biographyData.q2}. ` +
                  `Profession: ${biographyData.q3}. ` +
                  `Personality: ${biographyData.q4}. ` +
                  `Notable achievement: ${biographyData.q5}. ` +
                  `Format it beautifully with proper paragraphs.`;
    
    try {
        const response = await fetch('/api/chat/biography-assistance', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                message: prompt,
                conversationHistory: []
            })
        });
        
        const data = await response.json();
        
        if (response.ok) {
            generatedText = data.response;
            document.getElementById('generatedBiography').innerHTML = `
                <div style="text-align: justify;">${generatedText.replace(/\n/g, '<br><br>')}</div>
            `;
        } else {
            document.getElementById('generatedBiography').innerHTML = `
                <div class="alert alert-danger">${data.error || 'Error generating biography'}</div>
            `;
        }
    } catch (error) {
        document.getElementById('generatedBiography').innerHTML = `
            <div class="alert alert-danger">Connection error. Please try again.</div>
        `;
    }
}

function regenerateBiography() {
    document.getElementById('generatedBiography').innerHTML = `
        <div class="text-center">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2">Regenerating your biography...</p>
        </div>
    `;
    generateBiography();
}

function confirmBiography() {
    if (confirm('Do you want to use this biography?')) {
        document.getElementById('Biography').value = generatedText;
        document.getElementById('aiAssistantContainer').style.display = 'none';
        alert('Biography has been applied successfully!');
        
        // Reset for next use
        document.getElementById('previewPhase').style.display = 'none';
        document.getElementById('questionsPhase').style.display = 'block';
        document.getElementById('q1').value = '';
        document.getElementById('q2').value = '';
        document.getElementById('q3').value = '';
        document.getElementById('q4').value = '';
        document.getElementById('q5').value = '';
    }
}