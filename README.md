Libary küsimused

* Milline on DbContext-i ülesanne?
See on sild koodi ja andmebaasi vahel, mis vahendab päringuid, haldab ühendust ja salvestab muudatused andmebaasi.
Miks

* Miks ei ole hea paigutada kõiki andmebaasipäringuid otse kasutajaliidese koodi?
See muudab koodi segaseks ja raskesti muudetavaks. Kui andmebaas muutub, peab muidu hakkama koodi parandama paljudel erinevatel ekraanidel ja vaadetes.

* Millisesse kihti paigutaksid andmebaasiga suhtlemise MVC- või VIPER-arhitektuuris? Põhjenda oma vastust.
MVC-s: Model-kihti (või teenuste kihti, nagu meil LibaryServices).
  
VIPER-is: Interactor-kihti, mis tegeleb äriloogikaga.
  
Põhjendus: Nii on kood eraldatud – ekraan (View) tegeleb ainult ilusa pildi kuvamisega ja ei pea teadma, kuidas andmeid andmebaasist loetakse või salvestatakse.
