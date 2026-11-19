import type { LucideIcon } from "lucide-react";

export type ServiceColor = "mint" | "blue" | "peach";

export interface ServiceDefinition {
  icon: LucideIcon;
  number: string;
  titleKey: string;
  descriptionKey: string;
  color: ServiceColor;
}

export interface ServiceStepContent {
  title: string;
  description: string;
  titleAr: string;
  descriptionAr: string;
}

export interface ServiceFaqContent {
  question: string;
  answer: string;
  questionAr: string;
  answerAr: string;
}

export interface ServiceDetailsContent {
  aboutTitle: string;
  aboutText: string;
  aboutTitleAr: string;
  aboutTextAr: string;

  audienceTitle: string;
  audienceDescription: string;
  audienceTitleAr: string;
  audienceDescriptionAr: string;
  audienceItems: string[];
  audienceItemsAr: string[];

  includedTitle: string;
  includedDescription: string;
  includedTitleAr: string;
  includedDescriptionAr: string;
  includedItems: string[];
  includedItemsAr: string[];

  processTitle: string;
  processDescription: string;
  processTitleAr: string;
  processDescriptionAr: string;
  steps: ServiceStepContent[];

  faqTitle: string;
  faqDescription: string;
  faqTitleAr: string;
  faqDescriptionAr: string;
  faqs: ServiceFaqContent[];
}

export interface ServiceResponse {
  id: string;
  title: string;
  name: string;
  description: string;
  points: string[];
  titleAr: string;
  nameAr: string;
  descriptionAr: string;
  pointsAr: string[];
  details: ServiceDetailsContent;
  status: string;
  createdAtUtc: string;
}
