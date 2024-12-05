using Vosk
using HTTP
using JSON

# Function to recognize speech
function recognize_speech()
    model = Vosk.Model(MODEL_PATH)
    rec = Vosk.Recognizer(model, 16000.0)

    # Open audio input (microphone)
    audio_input = Vosk.AudioInput()

    println("Assistant ready. Speak...")

    while true
        audio_chunk = read(audio_input, 4096)
        if length(audio_chunk) == 0
            break
        end
        
        if Vosk.accept_waveform(rec, audio_chunk)
            result = Vosk.result(rec)
            command = JSON.parse(result)["text"]
            println("Command received: $command")
            return command
        else
            partial_result = Vosk.partial_result(rec)
            println("Partial result: $(JSON.parse(partial_result)["partial"])")
        end
    end

    return ""
end

# Function to analyze command using LUIS
function analyze_command(command)
    if isempty(command)
        println("Command cannot be null or empty.")
        return ("", Dict())
    end

    url = "$LUIS_ENDPOINT/luis/v2.0/apps/$LUIS_APP_ID?subscription-key=$LUIS_API_KEY&verbose=true&timezoneOffset=0&q=$(URI.escape(command))"
    
    response = HTTP.get(url)
    data = JSON.parse(String(response.body))

    intent = data["topScoringIntent"]["intent"]
    entities = Dict{String, String}()

    for entity in data["entities"]
        entities[entity["type"]] = entity["entity"]
    end

    return (intent, entities)
end

# Main function
function main()
    while true
        command = recognize_speech()
        if !isempty(command)
            (intent, entities) = analyze_command(command)
            println("Predicted Intent: $intent")
            println("Entities: $entities")
        end
    end
end

main()


#Ce code fournit une interface de reconnaissance vocale qui permet à l'utilisateur de donner des commandes orales, lesquelles sont ensuite interprétées et analysées pour déterminer l'intention et les éléments clés, facilitant ainsi l'interaction avec un assistant vocal ou une application de commande vocale.