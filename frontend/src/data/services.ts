import { Activity, HeartHandshake, HeartPulse } from "lucide-react";
import type { ServiceDefinition } from "../types/service";

export const serviceDefinitions: ServiceDefinition[] = [
  {
    icon: HeartPulse,
    number: "01",
    titleKey: "services.home.title",
    descriptionKey: "services.home.description",
    color: "mint",
  },
  {
    icon: Activity,
    number: "02",
    titleKey: "services.physio.title",
    descriptionKey: "services.physio.description",
    color: "blue",
  },
  {
    icon: HeartHandshake,
    number: "03",
    titleKey: "services.medical.title",
    descriptionKey: "services.medical.description",
    color: "peach",
  },
];
