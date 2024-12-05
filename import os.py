import os
import subprocess
from datetime import datetime

# Fonction pour capturer l'arborescence si la date cor
# respond au 30 novembre 2024
def capture_tree():
    # Vérification de la date actuelle
    current_date = datetime.now().strftime("%d/%m/%Y")
    
    # Vérification si la date est le 30/11/2024
    if current_date == "30/11/2024":
        print(f"Capture de l'arborescence de C:\\ le {current_date} de 9h00 à 10h00")
        
        # Commande 'tree' pour générer l'arborescence de C:\ et enregistrer dans un fichier
        command = ['tree', 'C:\\', '/f', '/a']
        with open(r'C:\arbre_30_11_2024_9h_10h.txt', 'w') as file:
            subprocess.run(command, stdout=file, text=True)
        print("L'arborescence a été enregistrée dans C:\\arbre_30_11_2024_9h_10h.txt")
    else:
        print(f"La date actuelle ({current_date}) ne correspond pas à 30/11/2024. Aucune action effectuée.")

# Exécuter la fonction
if __name__ == "__main__":
    capture_tree()
